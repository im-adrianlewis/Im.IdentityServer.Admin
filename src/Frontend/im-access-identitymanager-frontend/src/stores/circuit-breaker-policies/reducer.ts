import { Reducer } from 'redux';
import { CircuitBreakerPolicyState, CircuitBreakerPolicyActionTypes } from './types';

const initialState: CircuitBreakerPolicyState = {
  data: undefined,
  error: undefined,
  loading: false
};

const reducer: Reducer<CircuitBreakerPolicyState> = (state = initialState, action) => {
  switch(action.type) {
    case CircuitBreakerPolicyActionTypes.FETCH_REQUEST:
      return { ...state, loading: true, id: action.payload };
    
    case CircuitBreakerPolicyActionTypes.FETCH_SUCCESS:
      return { ...state, loading: false, data: action.payload };

    case CircuitBreakerPolicyActionTypes.FETCH_ERROR:
      return { ...state, loading: false, error: action.payload };

    default:
      return state;
  }
};

export { reducer as CircuitBreakerPolicyReducer };
