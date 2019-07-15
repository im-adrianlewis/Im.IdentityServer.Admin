import { action } from 'typesafe-actions';
import { AnalyticsActionTypes } from './types';

export const analyticsRequest = (userId?: string) =>
  action(AnalyticsActionTypes.ANALYTICS_REQUEST, userId);

export const analyticsSuccess = () =>
  action(AnalyticsActionTypes.ANALYTICS_SUCCESS);

export const analyticsSkipped = () =>
  action(AnalyticsActionTypes.ANALYTICS_SKIPPED);
