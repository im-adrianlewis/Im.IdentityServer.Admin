import { Reducer } from 'redux';
import { ConversationState, ConversationActionTypes } from './types';

const initialState: ConversationState = {
  id: undefined,
  data: undefined,
  error: undefined,
  loading: false
};

const reducer: Reducer<ConversationState> = (state = initialState, action) => {
  switch(action.type) {
    case ConversationActionTypes.FETCH_REQUEST:
      return { ...state, loading: true, id: action.payload };
    
    case ConversationActionTypes.FETCH_SUCCESS:
      return { ...state, loading: false, data: action.payload };

    case ConversationActionTypes.FETCH_ERROR:
      return { ...state, loading: false, error: action.payload };

    default:
      return state;
  }
};

export { reducer as ConversationReducer };
