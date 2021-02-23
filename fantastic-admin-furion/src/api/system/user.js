import api from '@/api/index'
const apiHost = process.env.VUE_APP_API_ROOT

// 列表
export const getList = {
    url: `${apiHost}/api/system/getuserlist`, // 接口地址
    request(data) {
        return api({
            url: this.url,
            method: 'post',
            data: data
        })
    }
}