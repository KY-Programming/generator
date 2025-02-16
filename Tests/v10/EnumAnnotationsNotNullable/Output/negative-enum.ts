/* eslint-disable */
// tslint:disable

export enum NegativeEnum {
    value1 = -1,
    value2 = -2,
    value0 = 0
}

export const NegativeEnumValues = [-1, -2, 0];
export const NegativeEnumNames = ["Value1", "Value2", "Value0"];
export const NegativeEnumValueMapping: { [key: number]: string } = { -1: "Value1", -2: "Value2", 0: "Value0" };
export const NegativeEnumNameMapping: { [key: string]: number } = { "Value1": -1, "Value2": -2, "Value0": 0 };

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
