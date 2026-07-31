/* eslint-disable */
// tslint:disable

import { NullableDisabledTypes } from "./nullable-disabled-types";

export interface NullableEnabledTypes {
    nullableString?: string;
    nonNullableString: string;
    nullableDisabledTypes: NullableDisabledTypes | undefined;
}

// outputid:c5722132-ab5a-4d1f-8673-2b3afb959baf
