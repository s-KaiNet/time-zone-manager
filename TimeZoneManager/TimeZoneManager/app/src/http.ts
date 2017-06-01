import axios from 'axios';
import nprogress from 'nprogress';
import User from './services/User';

let http: Axios.AxiosInstance = axios.create({
    baseURL: window.location.origin,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
});

http.interceptors.request.use(config => {
    nprogress.start();
    let user: User = new User();

    if (user.isAuthenticated()) {
        config.headers = config.headers || {};
        config.headers['Authorization'] = 'Bearer ' + user.token;
    }
    return config;
});

http.interceptors.response.use(response => {
    nprogress.done();
    return response;
}, error => {
    nprogress.done();
    throw error;
});
export default http;
