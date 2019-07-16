export const AnalyticsActionTypes = {
  ANALYTICS_REQUEST: '@@Analytics/REQUEST',
  ANALYTICS_SUCCESS: '@@Analytics/SUCCESS',
  ANALYTICS_SKIPPED: '@@Analytics/SKIPPED'
};

export interface AnalyticsState {
  initialising: boolean;
  initialised: boolean;
  skipped: boolean;
}