/* eslint-disable */
// tslint:disable

import { GenericSubType } from "./generic-sub-type";
import { SubType } from "./sub-type";

export class Types {
    public static readonly constString: string = "String";
    public stringField: string = "";
    public stringProperty: string = "";
    public intProperty: number = 0;
    public boolProperty: boolean = false;
    public doubleProperty: number = 0;
    public nullableIntProperty?: number;
    public nullable2BoolProperty?: boolean;
    public dateTimeProperty: Date = new Date(0);
    public guidProperty: string = "00000000-0000-0000-0000-000000000000";
    public decimalProperty: number = 0;
    public stringArrayProperty: string[] = [];
    public subTypeList: SubType[] = [];
    public subTypeDictionary: Record<string, SubType> = {};
    public subTypeProperty: SubType | undefined;
    public genericSubType: GenericSubType<string, number> | undefined;
    public readonlyProperty: string = "";

    public constructor(init?: Partial<Types>) {
        Object.assign(this, init);
    }
}

// outputid:8d02ce37-8013-4a57-a40d-4a10c8c65de2
