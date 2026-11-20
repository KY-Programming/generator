/* eslint-disable */

import { ForcedInterfaceDetail } from './forced-interface-detail';
import { PreferClassesDetail } from './prefer-classes-detail';

export class PreferClassesModel {
  public name: string = '';
  public detail: PreferClassesDetail | undefined;
  public forced: ForcedInterfaceDetail | undefined;

  public constructor(init?: Partial<PreferClassesModel>) {
    Object.assign(this, init);
  }
}

// outputid:2a7c4f11-93be-4f0d-8a1e-6d4b0c58e372
