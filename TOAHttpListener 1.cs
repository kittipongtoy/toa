----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

12/17/2023 6:37:26 AM ------------------------------------------------------------------------------------------------------------------------------------------------------

Exception EUaConnectException: Failed to retrieve endpoints. Failed to connect to server opc.tcp://localhost:4840. StatusCode=Bad_Disconnect (0x80AD0000 - The server has disconnected from the client.)
  Exception occured at $0000000002728609 (Module "ProsysOPC.UaClient", Procedure "ProsysOPC.UaClient.TUaClient.DiscoverEndpoints", Unit "ProsysOPC.UaClient.pas", Line 7072)

Callstack:
(0000000001E17609){cess.exe    } [0000000002728609] ProsysOPC.UaClient.TUaClient.DiscoverEndpoints (Line 7072, "ProsysOPC.UaClient.pas" + 56) + $35
(00000000000601DB){cess.exe    } [00000000009711DB] System.SysUtils.RaiseExceptObject + $2B
(0000000000012146){cess.exe    } [0000000000923146] System.@RaiseAtExcept + $E6
(0000000000012181){cess.exe    } [0000000000923181] System.@RaiseExcept + $11
(0000000001E17609){cess.exe    } [0000000002728609] ProsysOPC.UaClient.TUaClient.DiscoverEndpoints (Line 7072, "ProsysOPC.UaClient.pas" + 56) + $35
(0000000001E18CD3){cess.exe    } [0000000002729CD3] ProsysOPC.UaClient.TUaClient.GetEndpoints (Line 7348, "ProsysOPC.UaClient.pas" + 8) + $16
(0000000001E1D9F5){cess.exe    } [000000000272E9F5] ProsysOPC.UaClient.TUaClient.InitEndpoint (Line 8085, "ProsysOPC.UaClient.pas" + 11) + $0
(00000000021CDEE2){cess.exe    } [0000000002ADEEE2] ProsysOPC.UaServer.TUaLocalRegistrationClient.RegisterServer2 (Line 12389, "ProsysOPC.UaServer.pas" + 8) + $0
(00000000021CABB5){cess.exe    } [0000000002ADBBB5] ProsysOPC.UaServer.TUaServer.RegisterServer (L