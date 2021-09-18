/* eslint-disable */
// tslint:disable

export class SelfReferencingType {
    public stringProperty: string;
    public selfProperty: SelfReferencingType;
    public selfList: SelfReferencingType[];
}

// outputid:605e91d7-ee13-4f1d-9b92-845bb3ace852
