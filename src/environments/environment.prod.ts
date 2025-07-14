export const environment = {
  production: true,
  apiUrl: 'https://api.example.com/api',
  websocketUrl: 'wss://api.example.com/ws',
  useMockData: false,
  mockDataDelay: 0,
  features: {
    workOrders: true,
    employees: true,
    manpower: true,
    equipment: true,
    materials: true,
    activityLogs: true
  },
  pagination: {
    defaultPageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100]
  },
  auth: {
    tokenKey: 'auth_token',
    refreshTokenKey: 'refresh_token',
    tokenExpirationKey: 'token_expiration'
  },
  upload: {
    maxFileSize: 10 * 1024 * 1024, // 10MB
    allowedFileTypes: ['image/jpeg', 'image/png', 'application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document']
  },
  notifications: {
    enabled: true,
    duration: 5000
  }
}; 