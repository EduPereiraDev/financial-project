import api from './api';
import {
  Invitation,
  InvitationListItem,
  CreateInvitationRequest,
  AcceptInvitationRequest,
} from '../types/invitation';

export const invitationService = {
  async create(data: CreateInvitationRequest): Promise<Invitation> {
    const response = await api.post('/invitations', data);
    return response.data;
  },

  async getByAccount(accountId: string): Promise<InvitationListItem[]> {
    const response = await api.get(`/invitations/account/${accountId}`);
    return response.data;
  },

  async getByToken(token: string): Promise<Invitation> {
    const response = await api.get(`/invitations/token/${token}`);
    return response.data;
  },

  async accept(data: AcceptInvitationRequest): Promise<void> {
    await api.post('/invitations/accept', data);
  },

  async cancel(invitationId: string): Promise<void> {
    await api.delete(`/invitations/${invitationId}`);
  },
};
