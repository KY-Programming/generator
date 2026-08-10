/* eslint-disable */
// tslint:disable

import { SubType } from "./sub-type";

export class BackgroundTypes {
    public stringProperty: string = "";
    public intProperty: number = 0;
    public dateTimeProperty: Date = new Date(0);
    public subTypeProperty: SubType | undefined;
    public subTypeList: SubType[] = [];
    public subTypeDictionary: Record<string, SubType> = {};

    public constructor(init?: Partial<BackgroundTypes>) {
        Object.assign(this, init);
    }
}

// outputid:84eb1514-1fc9-499f-924d-4a93a076f40a
