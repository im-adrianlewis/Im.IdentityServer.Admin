export interface CircuitBreakerPolicy {
  service: string;
  policyKey: string;
  isIsolated: string;
  lastUpdated: Date;
}

export interface CircuitBreakerPolicyRequest {
}

export interface CircuitBreakerPolicyResponse {
  policies: CircuitBreakerPolicy[];
}

export enum CircuitBreakerPolicyActionTypes {
  FETCH_REQUEST = '@@CircuitBreakerPolicy/FETCH_REQUEST',
  FETCH_SUCCESS = '@@CircuitBreakerPolicy/FETCH_SUCCESS',
  FETCH_ERROR = '@@CircuitBreakerPolicy/FETCH_ERROR'
}

export interface CircuitBreakerPolicyState {
  readonly loading: boolean;
  readonly data?: CircuitBreakerPolicy[];
  readonly error?: string;
}
