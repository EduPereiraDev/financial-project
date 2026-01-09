import { useState } from 'react'
import { Filter, X } from 'lucide-react'
import { Button } from './button'
import { motion, AnimatePresence } from 'framer-motion'

interface FilterOption {
  label: string
  value: string
}

interface FilterDropdownProps {
  label: string
  options: FilterOption[]
  value: string
  onChange: (value: string) => void
  icon?: React.ReactNode
}

export function FilterDropdown({ label, options, value, onChange, icon }: FilterDropdownProps) {
  const [isOpen, setIsOpen] = useState(false)

  const selectedOption = options.find(opt => opt.value === value)

  const handleSelect = (optionValue: string) => {
    onChange(optionValue)
    setIsOpen(false)
  }

  const handleClear = (e: React.MouseEvent) => {
    e.stopPropagation()
    onChange('')
  }

  return (
    <div className="relative">
      <Button
        variant="outline"
        onClick={() => setIsOpen(!isOpen)}
        className={`h-11 px-4 border-gray-300 hover:border-blue-500 transition-all ${
          value ? 'border-blue-500 bg-blue-50' : ''
        }`}
      >
        {icon || <Filter className="w-4 h-4 mr-2" />}
        <span className="mr-2">{label}</span>
        {selectedOption && (
          <>
            <span className="font-semibold text-blue-600">: {selectedOption.label}</span>
            <button
              onClick={handleClear}
              className="ml-2 p-0.5 hover:bg-blue-100 rounded-full transition-colors"
              type="button"
            >
              <X className="w-3 h-3" />
            </button>
          </>
        )}
      </Button>

      <AnimatePresence>
        {isOpen && (
          <>
            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
              className="fixed inset-0 z-40"
              onClick={() => setIsOpen(false)}
            />
            <motion.div
              initial={{ opacity: 0, y: -10 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -10 }}
              className="absolute top-full mt-2 left-0 z-50 w-56 bg-white rounded-lg shadow-xl border border-gray-200 py-2"
            >
              {options.map((option) => (
                <button
                  key={option.value}
                  onClick={() => handleSelect(option.value)}
                  className={`w-full px-4 py-2 text-left hover:bg-gray-100 transition-colors ${
                    value === option.value ? 'bg-blue-50 text-blue-600 font-medium' : 'text-gray-700'
                  }`}
                >
                  {option.label}
                </button>
              ))}
            </motion.div>
          </>
        )}
      </AnimatePresence>
    </div>
  )
}
