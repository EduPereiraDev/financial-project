export type AccountRole = 'Owner' | 'Editor' | 'Viewer';

export type InvitationStatus = 'Pending' | 'Accepted' | 'Rejected' | 'Expired' | 'Cancelled';

export interface Invitation {
  id: string;
  accountId: string;
  accountName: string;
  invitedByUserId: string;
  invitedByUserName: string;
  invitedEmail: string;
  role: AccountRole;
  status: InvitationStatus;
  expiresAt: string;
  createdAt: string;
  acceptedAt?: string;
}

export interface InvitationListItem {
  id: string;
  invitedEmail: string;
  role: AccountRole;
  status: InvitationStatus;
  expiresAt: string;
  createdAt: string;
  invitedByUserName: string;
}

export interface CreateInvitationRequest {
  accountId: string;
  invitedEmail: string;
  role: AccountRole;
}

export interface AcceptInvitationRequest {
  token: string;
}

export const ROLE_LABELS: Record<AccountRole, string> = {
  Owner: 'Proprietário',
  Editor: 'Editor',
  Viewer: 'Visualizador',
};

export const ROLE_DESCRIPTIONS: Record<AccountRole, string> = {
  Owner: 'Controle total da conta',
  Editor: 'Pode adicionar e editar transações',
  Viewer: 'Apenas visualizar',
};

export const STATUS_LABELS: Record<InvitationStatus, string> = {
  Pending: 'Pendente',
  Accepted: 'Aceito',
  Rejected: 'Rejeitado',
  Expired: 'Expirado',
  Cancelled: 'Cancelado',
};
