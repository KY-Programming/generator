﻿/* eslint-disable */
// tslint:disable

import { TypesGenericSubType } from "./types-generic-sub-type";
import { TypesSubType } from "./types-sub-type";

export class Types {
    public static readonly constString: string = "String";
    public static readonly constShort: number = 1;
    public static readonly constUShort: number = 2;
    public static readonly constInt: number = 3;
    public static readonly constUInt: number = 4;
    public static readonly constLong: number = 5;
    public static readonly constULong: number = 6;
    public static readonly constFloat: number = 7.1;
    public static readonly constDouble: number = 8.2;
    public static readonly constBool: boolean = true;
    public stringField?: string;
    public intField?: number = 0;
    public dateTimeField?: Date = new Date(0);
    public defaultString?: string | undefined = "Default";
    public defaultShort: number = 1;
    public defaultUShort: number = 2;
    public defaultInt: number = 3;
    public defaultUInt: number = 4;
    public defaultLong: number = 5;
    public defaultULong: number = 6;
    public defaultFloat: number = 7.1;
    public defaultDouble: number = 8.2;
    public defaultBool: boolean = true;
    public defaultNullableString?: string | undefined = "Default";
    public defaultNullableShort?: number | undefined = 1;
    public defaultNullableUShort?: number | undefined = 2;
    public defaultNullableInt?: number | undefined = 3;
    public defaultNullableUInt?: number | undefined = 4;
    public defaultNullableLong?: number | undefined = 5;
    public defaultNullableULong?: number | undefined = 6;
    public defaultNullableFloat?: number | undefined = 7.1;
    public defaultNullableDouble?: number | undefined = 8.2;
    public defaultNullableBool?: boolean | undefined = true;
    public stringProperty?: string | undefined;
    public shortProperty: number = 0;
    public uShortProperty: number = 0;
    public intProperty: number = 0;
    public uIntProperty: number = 0;
    public longProperty: number = 0;
    public uLongProperty: number = 0;
    public floatProperty: number = 0;
    public doubleProperty: number = 0;
    public boolProperty: boolean = false;
    public objectProperty?: unknown | undefined;
    public nullableBoolProperty?: boolean | undefined;
    public nullableShortProperty?: number | undefined;
    public nullableUShortProperty?: number | undefined;
    public nullableIntProperty?: number | undefined;
    public nullableUIntProperty?: number | undefined;
    public nullableLongProperty?: number | undefined;
    public nullableULongProperty?: number | undefined;
    public nullableFloatProperty?: number | undefined;
    public nullableDoubleProperty?: number | undefined;
    public nullable2BoolProperty?: boolean | undefined;
    public nullable2ShortProperty?: number | undefined;
    public nullable2UShortProperty?: number | undefined;
    public nullable2IntProperty?: number | undefined;
    public nullable2UIntProperty?: number | undefined;
    public nullable2LongProperty?: number | undefined;
    public nullable2ULongProperty?: number | undefined;
    public nullable2FloatProperty?: number | undefined;
    public nullable2DoubleProperty?: number | undefined;
    public systemStringProperty?: string | undefined;
    public systemInt16Property: number = 0;
    public systemInt32Property: number = 0;
    public systemInt64Property: number = 0;
    public systemSingleProperty: number = 0;
    public systemDoubleProperty: number = 0;
    public systemDateTimeProperty: Date = new Date(0);
    public systemArrayProperty?: [] | undefined;
    public systemByteProperty: number = 0;
    public systemCharProperty: number = 0;
    public systemDecimalProperty: number = 0;
    public systemGuidProperty: string;
    public systemObjectProperty?: unknown | undefined;
    public systemTimeSpanProperty: string;
    public systemUInt16Property: number = 0;
    public systemUInt32Property: number = 0;
    public systemUInt64Property: number = 0;
    public typesSubTypeProperty?: TypesSubType | undefined;
    public stringArrayProperty?: string[] | undefined;
    public intArrayProperty?: number[] | undefined;
    public byteArrayProperty?: number[] | undefined;
    public systemDateTimeArrayProperty?: Date[] | undefined;
    public subTypeArrayProperty?: TypesSubType[] | undefined;
    public stringList?: string[] | undefined;
    public subTypeList?: TypesSubType[] | undefined;
    public stringStringDictionary?: { [key: string]: string; } | undefined;
    public intStringDictionary?: { [key: number]: string; } | undefined;
    public stringSubTypeDictionary?: { [key: string]: TypesSubType; } | undefined;
    public intSubTypeDictionary?: { [key: number]: TypesSubType; } | undefined;
    public subTypeStringDictionary?: { /* unsupported type for key. Expected string or number. Got 'TypesSubType'. */ } | undefined;
    public typesGenericSubType?: TypesGenericSubType<string, number> | undefined;
    public readonlyProperty?: string | undefined;
    public writeonlyProperty?: string | undefined;

    public constructor(init?: Partial<Types>) {
        Object.assign(this, init);
    }
}

// outputid:88210355-ddb5-4bf6-a426-81493e839d4e