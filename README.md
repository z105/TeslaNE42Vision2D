# Tesla NE42 2D 视觉检测系统

基于 **Cognex VisionPro ** 的 2D 机器视觉上位机系统，用于 Tesla NE42 产线零部件的**位置定位、极性判别和条码读取**。通过 **TCP/IP** 与 PLC 通信，接收检测指令并回传检测结果。

## 功能特性

- **多相机并发采集** — 支持多台 GigE Vision 相机同时触发，按 PLC 指令选择左/右/全部工位
- **VisionPro ToolBlock 视觉分析** — 条码读取、极性识别、坐标定位
- **九点标定** — 像素坐标到物理坐标的精确转换，参数持久化
- **PLC 通信** — TCP 协议，支持心跳、命令应答、结果上报
- **状态机驱动** — Stateless 有限状态机管理完整生命周期
- **数据存储** — FreeSql + SQLite 本地存储检测记录
- **在线配置** — 相机、ToolBlock、标定参数均通过 UI 配置，JSON 持久化
- **Mock 模式** — 无需硬件即可软件调试


## 通信协议

### 连接方式

| 项目 | 说明 |
|------|------|
| 协议 | TCP/IP，上位机作为客户端连接 PLC |
| 端口 | 配置文件配置 |
| 数据包 |  |
| 心跳 | 每 1 秒发送，3 秒超时断开 |
| 重连 | 自动重连 |

### PLC → 视觉（命令包，971 字节）

| 偏移 | 长度 | 类型 | 说明 |
|------|------|------|------|
| 0 | 81 | string | 命令字符串 |
| 81 | 81×10 | string[10] | 字符串参数 |
| 891 | 8×10 | double[10] | 浮点参数 |

**支持的命令：**

| 命令 | 说明 |
|------|------|
| `Heartbeat` | 心跳 |
| `JobParameters:{id}` | 下发工位参数 |
| `GoToState:Preoperational` | 复位 |
| `GoToState:Ready` | 准备 |
| `GoToState:SingleExecution` | 单次检测（`stringParams[2]`: `"10"`=左, `"01"`=右, `"11"`=全部） |
| `GoToState:ContinuousExecution` | 连续检测 |
| `GoToState:Halt` | 暂停 |
| `SetJobId:{id}` | 设置工位 ID |

### 视觉 → PLC（发送包）

| 包类型 | 类型码 | 用途 |
|--------|--------|------|
| StatusTcpPacket | 1 | 心跳/状态上报（1s 周期） |
| ResultsTcpPacket | 2 | 检测结果（OK/NG、坐标、条码、极性） |
| JobParameterRequestTcpPacket | 3 | 请求 PLC 下发工位参数 |
| CommandResponseTcpPacket | 5 | 命令应答确认 |

## 状态机


| 状态 | 说明 |
|------|------|
| Preoperational | 初始状态，等待配方加载 |
| Ready | 就绪，等待触发 |
| SingleExecution | 单步检测，执行一次后自动返回 Ready |
| Halted | 暂停，可复位 |
| Error | 错误，需人工介入 |

## 检测流程

1. PLC 发送 `GoToState:SingleExecution`（携带位置参数）
2. 根据位置筛选相机（左/右/全部），并发触发采集
3. 图像送入 VisionPro ToolBlock 执行检测（读码、极性、坐标）
4. 像素坐标经**九点标定**转换为物理坐标
5. 综合判定 OK/NG，组装 ResultsTcpPacket 发送 PLC
6. 检测记录写入 SQLite

## 配置

配置文件位于 `configs/` 目录：

### app.json

```json
{
  "Ip": "192.168.1.10",
  "Port": 5000,
  "MachineID": "ABCD",
  "CameraCount": 2,
  "UseMock": false,
  "UseMockCamera": false,
  "MockImageFolder": "C:\\MockImages",
  "ImageSavePath": "C:\\SavedImages",
  "CalcToolBlockPath": "C:\\ToolBlocks\\CalcToolBlock.vpp",
  "DatabasePath": "Data Source=data.db",
  "AdminPassword": "1234",
  "ImageRetainDays": 30,
  "Cameras": [
    {
      "Index": 0,
      "Name": "Camera_Left",
      "Sn": "00-11-22-33-44-55",
      "Position": "left",
      "ExposurePolarity": 5000.0,
      "ExposureBarcode": 3000.0,
      "ToolBlockPath": "C:\\ToolBlocks\\Left.vpp",
      "VpNameX": "X",
      "VpNameY": "Y",
      "RecordKey": "LeftResult"
    }
  ]
}
```

| 关键配置 | 说明 |
|----------|------|
| `Ip` / `Port` | PLC 地址和端口 |
| `MachineID` | 设备标识（4 字符，与 PLC 约定） |
| `UseMock` | 启用 Mock 视觉服务 |
| `UseMockCamera` | 从本地图片读取代替相机采集 |
| `Cameras[].Sn` | 相机序列号（VisionPro 识别用） |
| `Cameras[].Position` | 工位：`left` / `right` |
| `Cameras[].ToolBlockPath` | 视觉工具文件 (.vpp) 路径 |



## 项目结构

```
TeslaNE42Vision2D/
├── Entity/                    # 数据实体（AppConfig, DetectRecord 等）
├── Packet/                    # TCP 包编解码（971 字节定长帧）
├── Services/                  # 核心业务
│   ├── AppStateMachine.cs     #   状态机
│   ├── ClientDevice.cs        #   设备入口
│   ├── Camera/                #   相机服务（VisionPro / Mock）
│   ├── Vision/                #   视觉服务（VisionPro / Mock）
│   └── Calibration/           #   九点标定
├── SocketTCPClient/           # TCP 异步客户端
├── Views/                     # WinForms 界面
├── Utils/                     # 工具类（日志、磁盘等）
├── configs/                   # 运行时配置
└── Program.cs                 # 入口
```

## 技术栈

| 技术 | 用途 |
|------|------|
| .NET Framework 4.8.1 / WinForms | 运行时与桌面界面 |
| Cognex VisionPro  | 图像采集、视觉分析、标定 |
| Stateless 5.20 | 有限状态机 |
| FreeSql 3.2 + SQLite | ORM + 本地存储 |
| Polly 8.6 | TCP 重连退避策略 |
| Serilog 4.2 | 文件日志 |

### 运行环境

- Windows 10 / 11 x64
- .NET Framework 4.8.1 Runtime
- Cognex VisionPro SDK
- GigE Vision 兼容相机
