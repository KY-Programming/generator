/* eslint-disable */
// tslint:disable

export enum ConnectionStatus {
    connecting = 0,
    connected = 1,
    sleeping = 2,
    disconnected = 3
}

export const ConnectionStatusValues = [0, 1, 2, 3];
export const ConnectionStatusNames = ["connecting", "connected", "sleeping", "disconnected"];
export const ConnectionStatusValueMapping: { [key: number]: string } = { 0: "connecting", 1: "connected", 2: "sleeping", 3: "disconnected" };
export const ConnectionStatusNameMapping: { [key: string]: number } = { "connecting": 0, "connected": 1, "sleeping": 2, "disconnected": 3 };

// outputid:969bbdc9-df23-40ed-b6dd-702aa1fe7b03
