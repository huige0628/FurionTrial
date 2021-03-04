import Layout from '@/layout'

export default {
    path: '/base_info',
    component: Layout,
    redirect: '/base_info/country',
    name: 'baseInfo',
    meta: {
        title: '基础信息',
        icon: 'sidebar-component'
    },
    children: [{
        path: 'country',
        name: 'country',
        component: () =>
            import (/* webpackChunkName: 'baseInfo' */ '@/views/base_info/country/index'),
        meta: {
            title: '国家管理'
        }
    },
    {
        path: 'currency',
        name: 'currency',
        component: () =>
            import (/* webpackChunkName: 'baseInfo' */ '@/views/base_info/currency/index'),
        meta: {
            title: '币种管理'
        }
    },
    {
        path: 'exchangerate',
        name: 'exchangerate',
        component: () =>
            import (/* webpackChunkName: 'baseInfo' */ '@/views/base_info/exchangerate/index'),
        meta: {
            title: '汇率管理'
        }
    }
    ]
}
