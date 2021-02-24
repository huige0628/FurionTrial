const Mock = require('mockjs')

export default {
    'POST /mock/member/login': (req, res) => {
        return res.json(Mock.mock({
            error: '',
            state: 1,
            data: {
                account: req.body.account,
                token: '@string',
                failure_time: Date.parse(new Date()) / 1000 + 24 * 60 * 60
            }
        }))
    },
    'GET /mock/member/permission': (req, res) => {
        let permissions = []
        if (req.query.account == 'admin' || option.query.account == 'llh') {
            permissions = [
                'permission.browse',
                'permission.create',
                'permission.edit',
                'permission.remove'
            ]
        } else if (req.query.account == 'test') {
            permissions = [
                'permission.browse'
            ]
        }
        return res.json({
            error: '',
            state: 1,
            statusCode: 200,
            succeeded: true,
            data: {
                permissions
            }
        })
    }
}
