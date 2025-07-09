# Build stage
FROM node:20 AS build
WORKDIR /app
COPY . .
RUN npm install && npm run build --prod

# Serve stage
FROM nginx:alpine
COPY --from=build /app/dist/comments-app /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
