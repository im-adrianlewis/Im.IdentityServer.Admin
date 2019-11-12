import { Reducer } from 'redux';
import { ActiveBrandState, ActiveBrandActionTypes } from './types';

const initialState: ActiveBrandState = {
  id: '',
  data: undefined,
  error: undefined,
  loading: false
};

const reducer: Reducer<ActiveBrandState> = (state = initialState, action) => {
  switch(action.type) {
    case ActiveBrandActionTypes.FETCH_REQUEST:
      return { ...state, loading: true };
    
    case ActiveBrandActionTypes.UPDATE_BRAND:
      return { ...state, loading: true, id: action.payload };
    
    case ActiveBrandActionTypes.FETCH_SUCCESS:
      return { ...state, loading: false, data: action.payload };

    case ActiveBrandActionTypes.FETCH_ERROR:
      return { ...state, loading: false, error: action.payload };

    default:
      return state;
  }
};

export { reducer as ActiveBrandReducer };
