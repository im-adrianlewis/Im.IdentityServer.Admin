import { action } from 'typesafe-actions';
import { ConversationActionTypes, Conversation } from './types';

export const fetchRequest = (conversationId: string) =>
  action(ConversationActionTypes.FETCH_REQUEST, conversationId);

export const fetchSuccess = (data: Conversation) =>
  action(ConversationActionTypes.FETCH_SUCCESS, data);

export const fetchError = (message: string) =>
  action(ConversationActionTypes.FETCH_ERROR, message);
