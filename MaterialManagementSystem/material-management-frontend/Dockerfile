# Dockerfile for Material Management Frontend (React)
# Multi-stage build for optimized production image

# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Copy package files
COPY package.json pnpm-lock.yaml ./

# Install pnpm and dependencies
RUN npm install -g pnpm
RUN pnpm install --frozen-lockfile

# Copy source code
COPY . .

# Build the application
RUN pnpm run build

# Production stage with Nginx
FROM nginx:alpine AS final

# Install curl for health checks
RUN apk add --no-cache curl

# Copy built application
COPY --from=build /app/dist /usr/share/nginx/html

# Copy custom Nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost || exit 1

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]

