import Layout from '@/layout'

export default {
    path: '/system',
    component: Layout,
    redirect: '/system/user',
    name: 'systemManager',
    meta: {
        title: '系统管理',
        icon: 'sidebar-component'
    },
    children: [{
        path: 'user',
        name: 'user',
        component: () =>
            import (/* webpackChunkName: 'system' */ '@/views/system/user/index'),
        meta: {
            title: '用户管理'
        }
    },
    {
        path: 'role',
        name: 'role',
        component: () =>
            import (/* webpackChunkName: 'system' */ '@/views/system/role/index'),
        meta: {
            title: '角色管理'
        }
    }
    ]
}