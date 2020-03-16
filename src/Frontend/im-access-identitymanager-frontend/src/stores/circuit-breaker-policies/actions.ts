import { action } from 'typesafe-actions';
import { CircuitBreakerPolicyActionTypes, CircuitBreakerPolicy } from './types';

export const fetchRequest = () =>
  action(CircuitBreakerPolicyActionTypes.FETCH_REQUEST);

export const fetchSuccess = (data: CircuitBreakerPolicy[]) =>
  action(CircuitBreakerPolicyActionTypes.FETCH_SUCCESS, data);

export const fetchError = (message: string) =>
  action(CircuitBreakerPolicyActionTypes.FETCH_ERROR, message);
