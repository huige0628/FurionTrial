import axios from 'axios'
// import Qs from 'qs'
import router from '@/router/index'
import store from '@/store/index'
import { Message } from 'element-ui'

const toLogin = () => {
    router.push({
        path: '/login',
        query: {
            redirect: router.currentRoute.fullPath
        }
    })
}

const api = axios.create({
    baseURL: process.env.VUE_APP_API_ROOT,
    timeout: 10000,
    responseType: 'json'
    // withCredentials: true
})

api.interceptors.request.use(
    request => {
        if (request.method == 'post') {
            if (request.data instanceof FormData) {
                if (store.getters['user/isLogin']) {
                    // 如果是 FormData 类型（上传图片）
                    request.headers.Authorization = 'Bearer ' + store.state.user.token
                }
            } else {
                // 带上 token
                if (request.data == undefined) {
                    request.data = {}
                }
                if (store.getters['user/isLogin']) {
                    request.headers.Authorization = 'Bearer ' + store.state.user.token
                }
                // request.data = Qs.stringify(request.data)
            }
        } else {
            // 带上 token
            if (request.params == undefined) {
                request.params = {}
            }
            if (store.getters['user/isLogin']) {
                request.headers.Authorization = 'Bearer ' + store.state.user.token
            }
        }
        return request
    }
)

api.interceptors.response.use(
    response => {
        console.log(response)
        if (!response.data.succeeded) {
            // 如果接口请求时发现 token 失效，则立马跳转到登录页
            if (response.data.statusCode == 401) {
                toLogin()
                return false
            }
            Message.error(response.data.errors)
            return Promise.reject(response.data)
        }
        return Promise.resolve(response.data)
    },
    error => {
        console.log('request-error', error)
        return Promise.reject(error)
    }
)

export default api