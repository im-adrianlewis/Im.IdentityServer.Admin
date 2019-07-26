export interface ActiveBrand {
  availableBrands: string[];
  selectedBrand: string;
}

export interface ActiveBrandRequest {
}

export interface ActiveBrandUpdate {
  brand: string;
}

export interface ActiveBrandResponse {
  activeBrands: ActiveBrand;
}

export interface ActiveBrandVariables {
  conversationId: string;
}

export enum ActiveBrandActionTypes {
  FETCH_REQUEST = '@@ActiveBrand/FETCH_REQUEST',
  UPDATE_BRAND = '@@ActiveBrand/UPDATE_BRAND',
  FETCH_SUCCESS = '@@ActiveBrand/FETCH_SUCCESS',
  FETCH_ERROR = '@@ActiveBrand/FETCH_ERROR'

  // TODO: Add subscription action types
}

export interface ActiveBrandState {
  readonly id?: string;
  readonly loading: boolean;
  readonly data?: ActiveBrand;
  readonly error?: string;
}
