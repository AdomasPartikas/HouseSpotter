import axios from 'axios';
import Cookies from 'js-cookie';

const api = axios.create({
    baseURL: 'http://localhost:5287/',
    timeout: 10000,
});

api.interceptors.request.use(request => {
    const token = Cookies.get('jwt');
    if (token) {
        request.headers.Authorization = `Bearer ${token}`;
    }
    return request;
}, error => {
    return Promise.reject(error);
});

api.interceptors.response.use(response => {
    const newToken = response.headers['new-jwt'];
    if (newToken) {
        Cookies.set('jwt', newToken, { expires: 1 });
    }
    return response;
}, error => {
    return Promise.reject(error);
});

export default api;