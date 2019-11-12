import { action } from 'typesafe-actions';
import { ActiveBrandActionTypes, ActiveBrand } from './types';

export const fetchRequest = () =>
  action(ActiveBrandActionTypes.FETCH_REQUEST);

export const updateBrand = (currentBrand: string) =>
  action(ActiveBrandActionTypes.UPDATE_BRAND, currentBrand);

export const fetchSuccess = (data: ActiveBrand) =>
  action(ActiveBrandActionTypes.FETCH_SUCCESS, data);

export const fetchError = (message: string) =>
  action(ActiveBrandActionTypes.FETCH_ERROR, message);
