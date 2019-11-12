export interface UserSearchCriteria {
  firstName: string | undefined;
  lastName: string | undefined;
  email: string | undefined;
  screenName: string | undefined;
  createDateFrom: Date | undefined;
  createDateTo: Date | undefined;
  LastLoggedInDateFrom: Date | undefined;
  LastLoggedInDateTo: Date | undefined;
  pageIndex: number;
  pageSize: number;
}

export interface UserItem {
  id: string | undefined;
  tenantId: string | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  email: string | undefined;
  emailConfirmed: boolean | undefined;
  phoneNumber: string | undefined;
  phoneNumberConfirmed: boolean | undefined;
  lockoutEndDateUtc: Date | undefined;
  lockoutEnabled: boolean | undefined;
  accessFailedCount: number | undefined;
  userName: string | undefined;
  createDate: Date | undefined;
  lastUpdatedDate: Date | undefined;
  registrationDate: Date | undefined;
  lastLoggedInDate: Date | undefined;
  registrationIpAddress: string | undefined;
  lastLoggedInIpAddress: string | undefined;
  screenName: string | undefined;
  userType: string | undefined;
  address1: string | undefined;
  address2: string | undefined;
  city: string | undefined;
  county: string | undefined;
  country: string | undefined;
  postcode: string | undefined;
  userBiography: string | undefined;
  firstPartyIm: boolean | undefined;
  firstPartyImUpdatedDate: Date | undefined;
  authenticationType: string | undefined;
}

export interface PagedUsers {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  items: UserItem[];
}

export interface UserSearchRequest {
  criteria: UserSearchCriteria;
}

export interface UserSearchResponse {
  pagedUsers: PagedUsers;
}  

export interface UserSearchVariables {
  criteria: UserSearchCriteria;
}

export interface UserSelfResponse {
  user: UserItem;
}  

export enum UserQueryActionTypes {
  FETCH_SELF_REQUEST = '@@UserQuery/FETCH_SELF_REQUEST',
  FETCH_SELF_SUCCESS = '@@UserQuery/FETCH_SELF_SUCCESS',
  FETCH_USERS_REQUEST = '@@UserQuery/FETCH_USERS_REQUEST',
  FETCH_USERS_SUCCESS = '@@UserQuery/FETCH_USERS_SUCCESS',
  FETCH_ERROR = '@@UserQuery/FETCH_ERROR'
}

export interface UserQueryState {
  readonly id?: string;
  readonly loading: boolean;
  readonly self?: UserItem;
  readonly data?: PagedUsers;
  readonly error?: string;
}
