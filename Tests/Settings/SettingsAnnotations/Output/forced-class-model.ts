/* eslint-disable */

import { ForcedClassDetail } from './forced-class-detail';

export class ForcedClassModel {
  public name: string = '';
  public detail: ForcedClassDetail | undefined;

  public constructor(init?: Partial<ForcedClassModel>) {
    Object.assign(this, init);
  }
}

// outputid:2a7c4f11-93be-4f0d-8a1e-6d4b0c58e372
