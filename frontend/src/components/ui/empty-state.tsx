import { motion } from 'framer-motion'
import { LucideIcon } from 'lucide-react'
import { Button } from './button'

interface EmptyStateProps {
  icon: LucideIcon
  title: string
  description: string
  actionLabel?: string
  onAction?: () => void
  iconColor?: string
  iconBgColor?: string
}

export function EmptyState({
  icon: Icon,
  title,
  description,
  actionLabel,
  onAction,
  iconColor = 'text-gray-400',
  iconBgColor = 'bg-gray-100'
}: EmptyStateProps) {
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className="flex flex-col items-center justify-center py-12 px-4"
    >
      {/* Ícone Ilustrativo */}
      <motion.div
        initial={{ scale: 0 }}
        animate={{ scale: 1 }}
        transition={{ delay: 0.2, type: 'spring', stiffness: 200 }}
        className={`${iconBgColor} rounded-full p-6 mb-6`}
      >
        <Icon className={`w-16 h-16 ${iconColor}`} strokeWidth={1.5} />
      </motion.div>

      {/* Título */}
      <motion.h3
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ delay: 0.3 }}
        className="text-xl font-semibold text-gray-900 mb-2 text-center"
      >
        {title}
      </motion.h3>

      {/* Descrição */}
      <motion.p
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ delay: 0.4 }}
        className="text-gray-600 text-center max-w-md mb-6"
      >
        {description}
      </motion.p>

      {/* Botão de Ação (opcional) */}
      {actionLabel && onAction && (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.5 }}
        >
          <Button
            onClick={onAction}
            className="bg-gradient-to-r from-blue-500 to-purple-600 hover:from-blue-600 hover:to-purple-700 text-white shadow-lg hover:shadow-xl transition-all duration-300"
          >
            {actionLabel}
          </Button>
        </motion.div>
      )}
    </motion.div>
  )
}
