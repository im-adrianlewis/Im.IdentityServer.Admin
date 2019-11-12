import { Reducer } from 'redux';
import { UserQueryState, UserQueryActionTypes } from './types';

const initialState: UserQueryState = {
  id: '',
  self: undefined,
  data: undefined,
  error: undefined,
  loading: false
};

const reducer: Reducer<UserQueryState> = (state = initialState, action) => {
  switch(action.type) {
    case UserQueryActionTypes.FETCH_SELF_REQUEST:
      return { ...state, loading: true };
    
    case UserQueryActionTypes.FETCH_SELF_SUCCESS:
      return { ...state, loading: false, self: action.payload };

    case UserQueryActionTypes.FETCH_USERS_REQUEST:
      return { ...state, loading: true, criteria: action.payload };
    
    case UserQueryActionTypes.FETCH_USERS_SUCCESS:
      return { ...state, loading: false, data: action.payload };

    case UserQueryActionTypes.FETCH_ERROR:
      return { ...state, loading: false, error: action.payload };

    default:
      return state;
  }
};

export { reducer as UserQueryReducer };
