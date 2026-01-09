import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { invitationService } from '../services/invitationService';
import { Invitation, ROLE_LABELS, ROLE_DESCRIPTIONS } from '../types/invitation';

export default function AcceptInvitationPage() {
  const { token } = useParams<{ token: string }>();
  const navigate = useNavigate();
  const [invitation, setInvitation] = useState<Invitation | null>(null);
  const [loading, setLoading] = useState(true);
  const [accepting, setAccepting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadInvitation();
  }, [token]);

  const loadInvitation = async () => {
    if (!token) {
      setError('Token de convite inválido');
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      const data = await invitationService.getByToken(token);
      setInvitation(data);
    } catch (error: any) {
      console.error('Erro ao carregar convite:', error);
      setError(error.response?.data?.message || 'Convite não encontrado ou expirado');
    } finally {
      setLoading(false);
    }
  };

  const handleAccept = async () => {
    if (!token) return;

    try {
      setAccepting(true);
      await invitationService.accept({ token });
      alert('Convite aceito com sucesso! Você agora tem acesso à conta.');
      navigate('/dashboard');
    } catch (error: any) {
      console.error('Erro ao aceitar convite:', error);
      alert(error.response?.data?.message || 'Erro ao aceitar convite');
    } finally {
      setAccepting(false);
    }
  };

  const handleReject = () => {
    if (confirm('Tem certeza que deseja rejeitar este convite?')) {
      navigate('/dashboard');
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: 'long',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const isExpired = invitation && new Date(invitation.expiresAt) < new Date();

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gray-50">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Carregando convite...</p>
        </div>
      </div>
    );
  }

  if (error || !invitation) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gray-50">
        <div className="bg-white rounded-lg shadow-lg p-8 max-w-md w-full text-center">
          <div className="text-red-500 text-6xl mb-4">⚠️</div>
          <h1 className="text-2xl font-bold text-gray-800 mb-4">Convite Inválido</h1>
          <p className="text-gray-600 mb-6">{error}</p>
          <button
            onClick={() => navigate('/dashboard')}
            className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
          >
            Ir para Dashboard
          </button>
        </div>
      </div>
    );
  }

  if (isExpired) {
    return (
      <div className="flex justify-center items-center min-h-screen bg-gray-50">
        <div className="bg-white rounded-lg shadow-lg p-8 max-w-md w-full text-center">
          <div className="text-yellow-500 text-6xl mb-4">⏰</div>
          <h1 className="text-2xl font-bold text-gray-800 mb-4">Convite Expirado</h1>
          <p className="text-gray-600 mb-6">
            Este convite expirou em {formatDate(invitation.expiresAt)}.
            Entre em contato com {invitation.invitedByUserName} para receber um novo convite.
          </p>
          <button
            onClick={() => navigate('/dashboard')}
            className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
          >
            Ir para Dashboard
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-50 p-4">
      <div className="bg-white rounded-lg shadow-lg p-8 max-w-2xl w-full">
        <div className="text-center mb-8">
          <div className="text-blue-500 text-6xl mb-4">📨</div>
          <h1 className="text-3xl font-bold text-gray-800 mb-2">Convite para Conta Compartilhada</h1>
          <p className="text-gray-600">
            Você foi convidado por <span className="font-semibold">{invitation.invitedByUserName}</span>
          </p>
        </div>

        <div className="bg-gray-50 rounded-lg p-6 mb-6">
          <h2 className="text-xl font-semibold mb-4">Detalhes do Convite</h2>
          
          <div className="space-y-3">
            <div className="flex justify-between">
              <span className="text-gray-600">Conta:</span>
              <span className="font-semibold">{invitation.accountName}</span>
            </div>
            
            <div className="flex justify-between">
              <span className="text-gray-600">Seu email:</span>
              <span className="font-semibold">{invitation.invitedEmail}</span>
            </div>
            
            <div className="flex justify-between">
              <span className="text-gray-600">Nível de acesso:</span>
              <span className="font-semibold">{ROLE_LABELS[invitation.role]}</span>
            </div>
            
            <div className="flex justify-between">
              <span className="text-gray-600">Enviado em:</span>
              <span className="font-semibold">{formatDate(invitation.createdAt)}</span>
            </div>
            
            <div className="flex justify-between">
              <span className="text-gray-600">Expira em:</span>
              <span className="font-semibold">{formatDate(invitation.expiresAt)}</span>
            </div>
          </div>
        </div>

        <div className="bg-blue-50 border border-blue-200 rounded-lg p-4 mb-6">
          <h3 className="font-semibold text-blue-800 mb-2">
            {ROLE_LABELS[invitation.role]}
          </h3>
          <p className="text-blue-700 text-sm">
            {ROLE_DESCRIPTIONS[invitation.role]}
          </p>
        </div>

        <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mb-6">
          <p className="text-yellow-800 text-sm">
            <span className="font-semibold">⚠️ Atenção:</span> Ao aceitar este convite, você terá acesso
            às informações financeiras desta conta de acordo com o nível de permissão concedido.
          </p>
        </div>

        <div className="flex gap-4">
          <button
            onClick={handleReject}
            className="flex-1 px-6 py-3 border-2 border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition font-semibold"
            disabled={accepting}
          >
            Rejeitar
          </button>
          <button
            onClick={handleAccept}
            className="flex-1 bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition font-semibold disabled:opacity-50"
            disabled={accepting}
          >
            {accepting ? 'Aceitando...' : 'Aceitar Convite'}
          </button>
        </div>
      </div>
    </div>
  );
}
