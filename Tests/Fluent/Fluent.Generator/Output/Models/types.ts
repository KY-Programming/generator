// tslint:disable

import { SubType } from "./sub-type";

export class Types {
    public stringField: string;
    public intField: number;
    public dateTimeField: Date;
    public stringProperty: string;
    public shortProperty: number;
    public uShortProperty: number;
    public intProperty: number;
    public uIntProperty: number;
    public longProperty: number;
    public uLongProperty: number;
    public floatProperty: number;
    public doubleProperty: number;
    public boolProperty: boolean;
    public objectProperty: unknown;
    public nullableBoolProperty: boolean;
    public nullableShortProperty: number;
    public nullableUShortProperty: number;
    public nullableIntProperty: number;
    public nullableUIntProperty: number;
    public nullableLongProperty: number;
    public nullableULongProperty: number;
    public nullableFloatProperty: number;
    public nullableDoubleProperty: number;
    public nullable2BoolProperty: boolean;
    public nullable2ShortProperty: number;
    public nullable2UShortProperty: number;
    public nullable2IntProperty: number;
    public nullable2UIntProperty: number;
    public nullable2LongProperty: number;
    public nullable2ULongProperty: number;
    public nullable2FloatProperty: number;
    public nullable2DoubleProperty: number;
    public systemStringProperty: string;
    public systemInt16Property: number;
    public systemInt32Property: number;
    public systemInt64Property: number;
    public systemSingleProperty: number;
    public systemDoubleProperty: number;
    public systemDateTimeProperty: Date;
    public systemArrayProperty: [];
    public systemByteProperty: number;
    public systemCharProperty: number;
    public systemDecimalProperty: number;
    public systemGuidProperty: string;
    public systemObjectProperty: unknown;
    public systemTimeSpanProperty: string;
    public systemUInt16Property: number;
    public systemUInt32Property: number;
    public systemUInt64Property: number;
    public subTypeProperty: SubType;
    public stringArrayProperty: string[];
    public intArrayProperty: number[];
    public byteArrayProperty: number[];
    public systemDateTimeArrayProperty: Date[];
    public subTypeArrayProperty: SubType[];
    public stringList: string[];
    public subTypeList: SubType[];
    public stringStringDictionary: { [key: string]: string; };
    public intStringDictionary: { [key: number]: string; };
    public stringSubTypeDictionary: { [key: string]: SubType; };
    public intSubTypeDictionary: { [key: number]: SubType; };
    public subTypeStringDictionary: { /* unsupported type for key. Expected string or number. Got 'SubType' */. };
    public readonlyProperty: string;
    public writeonlyProperty: string;

    public constructor(init: Partial<Types> = undefined) {
        Object.assign(this, init);
    }
}

// outputid:cabb1031-51d3-4479-98b6-c8c0321d279a