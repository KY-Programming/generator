/* eslint-disable */
// tslint:disable

export enum FlagsEnum {
    none = 0,
    first = 1,
    second = 2,
    third = 4,
    firstAndSecond = 3
}

export const FlagsEnumValues = [0, 1, 2, 4, 3];
export const FlagsEnumNames = ["None", "First", "Second", "Third", "FirstAndSecond"];
export const FlagsEnumValueMapping: { [key: number]: string } = { 0: "None", 1: "First", 2: "Second", 4: "Third", 3: "FirstAndSecond" };
export const FlagsEnumNameMapping: { [key: string]: number } = { "None": 0, "First": 1, "Second": 2, "Third": 4, "FirstAndSecond": 3 };

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
