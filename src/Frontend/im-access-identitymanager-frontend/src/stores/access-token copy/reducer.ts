import { Reducer } from 'redux';
import { AccessTokenState, AccessTokenActionTypes } from './types';

const initialState: AccessTokenState = {
  id: '',
  data: undefined,
  error: undefined,
  loading: false
};

const reducer: Reducer<AccessTokenState> = (state = initialState, action) => {
  switch(action.type) {
    case AccessTokenActionTypes.FETCH_REQUEST:
      return { ...state, loading: true };
    
    case AccessTokenActionTypes.FETCH_SUCCESS:
      return { ...state, loading: false, data: action.payload };

    case AccessTokenActionTypes.FETCH_ERROR:
      return { ...state, loading: false, error: action.payload };

    default:
      return state;
  }
};

export { reducer as AccessTokenReducer };
