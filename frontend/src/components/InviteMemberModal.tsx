import { useState } from 'react';
import { invitationService } from '../services/invitationService';
import { AccountRole, ROLE_LABELS, ROLE_DESCRIPTIONS } from '../types/invitation';

interface InviteMemberModalProps {
  accountId: string;
  onClose: () => void;
  onSuccess: () => void;
}

export default function InviteMemberModal({ accountId, onClose, onSuccess }: InviteMemberModalProps) {
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    invitedEmail: '',
    role: 'Editor' as AccountRole,
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await invitationService.create({
        accountId,
        invitedEmail: formData.invitedEmail,
        role: formData.role,
      });
      onSuccess();
    } catch (error: any) {
      console.error('Erro ao enviar convite:', error);
      alert(error.response?.data?.message || 'Erro ao enviar convite');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-md w-full p-6">
        <h2 className="text-2xl font-bold mb-6">Convidar Membro</h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-2">Email</label>
            <input
              type="email"
              value={formData.invitedEmail}
              onChange={(e) => setFormData({ ...formData, invitedEmail: e.target.value })}
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
              placeholder="email@exemplo.com"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Nível de Acesso</label>
            <div className="space-y-3">
              {(['Owner', 'Editor', 'Viewer'] as AccountRole[]).map((role) => (
                <label
                  key={role}
                  className={`flex items-start p-4 border rounded-lg cursor-pointer transition ${
                    formData.role === role
                      ? 'border-blue-500 bg-blue-50'
                      : 'border-gray-200 hover:border-gray-300'
                  }`}
                >
                  <input
                    type="radio"
                    name="role"
                    value={role}
                    checked={formData.role === role}
                    onChange={(e) => setFormData({ ...formData, role: e.target.value as AccountRole })}
                    className="mt-1 mr-3"
                  />
                  <div className="flex-1">
                    <div className="font-medium">{ROLE_LABELS[role]}</div>
                    <div className="text-sm text-gray-600">{ROLE_DESCRIPTIONS[role]}</div>
                  </div>
                </label>
              ))}
            </div>
          </div>

          <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4 text-sm text-yellow-800">
            <p className="font-medium mb-1">⚠️ Atenção</p>
            <p>O convite será enviado para o email informado e expira em 7 dias.</p>
          </div>

          <div className="flex gap-4 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-6 py-2 border rounded-lg hover:bg-gray-50 transition"
              disabled={loading}
            >
              Cancelar
            </button>
            <button
              type="submit"
              className="flex-1 bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition disabled:opacity-50"
              disabled={loading}
            >
              {loading ? 'Enviando...' : 'Enviar Convite'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
