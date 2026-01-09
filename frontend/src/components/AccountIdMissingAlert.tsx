import { useNavigate } from 'react-router-dom';
import { useAuth } from '@/hooks/useAuth';

export default function AccountIdMissingAlert() {
  const navigate = useNavigate();
  const { logout } = useAuth();

  const handleRelogin = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <div className="max-w-md w-full bg-white rounded-lg shadow-lg p-8">
        <div className="text-center">
          <div className="mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-yellow-100 mb-4">
            <svg
              className="h-6 w-6 text-yellow-600"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
              />
            </svg>
          </div>
          
          <h3 className="text-lg font-medium text-gray-900 mb-2">
            Sessão Desatualizada
          </h3>
          
          <p className="text-sm text-gray-500 mb-6">
            Sua sessão precisa ser atualizada para acessar este recurso. 
            Por favor, faça login novamente.
          </p>
          
          <button
            onClick={handleRelogin}
            className="w-full bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition font-medium"
          >
            Fazer Login Novamente
          </button>
        </div>
      </div>
    </div>
  );
}
