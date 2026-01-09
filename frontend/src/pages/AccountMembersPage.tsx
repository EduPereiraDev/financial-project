import { useState, useEffect } from 'react';
import { invitationService } from '../services/invitationService';
import { InvitationListItem, ROLE_LABELS, STATUS_LABELS } from '../types/invitation';
import InviteMemberModal from '../components/InviteMemberModal';

interface AccountMember {
  id: string;
  userId: string;
  userName: string;
  userEmail: string;
  role: string;
  joinedAt: string;
}

export default function AccountMembersPage() {
  const [members] = useState<AccountMember[]>([]);
  const [invitations, setInvitations] = useState<InvitationListItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const accountId = localStorage.getItem('accountId') || '';

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      // Carregar membros (usando endpoint de account members que já existe)
      // Por enquanto vamos focar nos convites
      const invitationsData = await invitationService.getByAccount(accountId);
      setInvitations(invitationsData);
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCancelInvitation = async (invitationId: string) => {
    if (!confirm('Tem certeza que deseja cancelar este convite?')) return;

    try {
      await invitationService.cancel(invitationId);
      await loadData();
    } catch (error) {
      console.error('Erro ao cancelar convite:', error);
      alert('Erro ao cancelar convite');
    }
  };

  const handleInviteSuccess = async () => {
    setIsModalOpen(false);
    await loadData();
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR');
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Pending':
        return 'bg-yellow-100 text-yellow-800';
      case 'Accepted':
        return 'bg-green-100 text-green-800';
      case 'Expired':
        return 'bg-gray-100 text-gray-800';
      case 'Cancelled':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-xl">Carregando...</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Membros da Conta</h1>
        <button
          onClick={() => setIsModalOpen(true)}
          className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
        >
          + Convidar Membro
        </button>
      </div>

      {/* Seção de Membros Ativos */}
      <div className="mb-8">
        <h2 className="text-2xl font-semibold mb-4">Membros Ativos</h2>
        {members.length === 0 ? (
          <div className="bg-gray-50 rounded-lg p-6 text-center text-gray-500">
            Carregando membros...
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {members.map((member) => (
              <div key={member.id} className="bg-white rounded-lg shadow-md p-6 border-l-4 border-blue-500">
                <div className="flex items-start justify-between mb-2">
                  <div>
                    <h3 className="text-lg font-semibold">{member.userName}</h3>
                    <p className="text-sm text-gray-500">{member.userEmail}</p>
                  </div>
                  <span className="px-3 py-1 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                    {ROLE_LABELS[member.role as keyof typeof ROLE_LABELS] || member.role}
                  </span>
                </div>
                <p className="text-sm text-gray-600 mt-2">
                  Membro desde {formatDate(member.joinedAt)}
                </p>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Seção de Convites Pendentes */}
      <div>
        <h2 className="text-2xl font-semibold mb-4">Convites Pendentes</h2>
        {invitations.length === 0 ? (
          <div className="bg-gray-50 rounded-lg p-6 text-center text-gray-500">
            Nenhum convite pendente
          </div>
        ) : (
          <div className="space-y-4">
            {invitations.map((invitation) => (
              <div
                key={invitation.id}
                className="bg-white rounded-lg shadow-md p-6 flex items-center justify-between"
              >
                <div className="flex-1">
                  <div className="flex items-center gap-4 mb-2">
                    <h3 className="text-lg font-semibold">{invitation.invitedEmail}</h3>
                    <span className={`px-3 py-1 rounded-full text-xs font-medium ${getStatusColor(invitation.status)}`}>
                      {STATUS_LABELS[invitation.status]}
                    </span>
                    <span className="px-3 py-1 rounded-full text-xs font-medium bg-gray-100 text-gray-800">
                      {ROLE_LABELS[invitation.role]}
                    </span>
                  </div>
                  <div className="flex gap-4 text-sm text-gray-600">
                    <p>Convidado por: {invitation.invitedByUserName}</p>
                    <p>•</p>
                    <p>Enviado em: {formatDate(invitation.createdAt)}</p>
                    <p>•</p>
                    <p>Expira em: {formatDate(invitation.expiresAt)}</p>
                  </div>
                </div>
                {invitation.status === 'Pending' && (
                  <button
                    onClick={() => handleCancelInvitation(invitation.id)}
                    className="ml-4 px-4 py-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition"
                  >
                    Cancelar
                  </button>
                )}
              </div>
            ))}
          </div>
        )}
      </div>

      {isModalOpen && (
        <InviteMemberModal
          accountId={accountId}
          onClose={() => setIsModalOpen(false)}
          onSuccess={handleInviteSuccess}
        />
      )}
    </div>
  );
}
