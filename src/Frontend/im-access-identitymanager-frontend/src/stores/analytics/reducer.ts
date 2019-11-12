import { Reducer } from 'redux';
import { AnalyticsState, AnalyticsActionTypes } from './types';

const initialState: AnalyticsState = {
  initialising: false,
  initialised: false,
  skipped: false
};

const reducer: Reducer<AnalyticsState> = (state = initialState, action) => {
  switch(action.type) {
    case AnalyticsActionTypes.ANALYTICS_REQUEST:
      return { ...state, initialising: true, initialised: false, skipped: false };

    case AnalyticsActionTypes.ANALYTICS_SUCCESS:
      return { ...state, initialising: false, initialised: true };

    case AnalyticsActionTypes.ANALYTICS_SKIPPED:
      return { ...state, initialising: false, skipped: true };

    default:
      return state;
  }
};

export { reducer as AnalyticsReducer };
