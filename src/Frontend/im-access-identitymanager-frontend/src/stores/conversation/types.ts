export interface Conversation {
  id: string;
  fromPersonaId: string;
  fromPartyId: string;
  toPartyId: string;
  createdWhen: Date;
  lastUpdatedWhen: Date;
  messages: Message[];
}

export interface Message {
  id: string;
  fromPersonaId: string;
  fromPartyId: string;
  timestamp: Date;
  messageBody: string;
}

export interface ConversationRequest {
  conversationId: string;
}

export interface ConversationResponse {
  conversation: Conversation;
}

export interface ConversationVariables {
  conversationId: string;
}

export enum ConversationActionTypes {
  FETCH_REQUEST = '@@Conversation/FETCH_REQUEST',
  FETCH_SUCCESS = '@@Conversation/FETCH_SUCCESS',
  FETCH_ERROR = '@@Conversation/FETCH_ERROR'

  // TODO: Add subscription action types
}

// export const ConversationActionTypes = {
//   FETCH_REQUEST: '@@Conversation/FETCH_REQUEST',
//   FETCH_SUCCESS: '@@Conversation/FETCH_SUCCESS',
//   FETCH_ERROR: '@@Conversation/FETCH_ERROR'

//   // TODO: Add subscription action types
// };

export interface ConversationState {
  readonly id?: string;
  readonly loading: boolean;
  readonly data?: Conversation;
  readonly error?: string;
}
