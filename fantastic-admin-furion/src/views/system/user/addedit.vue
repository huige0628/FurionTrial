<template>
    <el-dialog
        :visible.sync="show"
        :close-on-click-modal="false"
        :title="title"
        width="50%"
        top="10vh"
        scrollable
    >
        <div
            style="max-height: 550px; overflow-x: auto;"
            class="form-container"
        >
            <el-form
                ref="form"
                :model="formData"
                :rules="formRules"
                label-width="100px"
            >
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="用户名" prop="userName">
                            <el-input
                                v-model="formData.userName"
                                placeholder="请输入用户名"
                                clearable
                            />
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item
                            v-if="!disabled"
                            label="登录密码"
                            prop="password"
                        >
                            <el-input
                                v-model="formData.password"
                                type="password"
                                placeholder="请输入密码"
                                :disabled="disabled"
                                clearable
                            />
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="邮箱" prop="email">
                            <el-input
                                v-model="formData.email"
                                placeholder="请输入邮箱"
                                clearable
                            />
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="手机" prop="phone">
                            <el-input
                                v-model.number="formData.phone"
                                placeholder="请输入手机号码"
                                maxlength="11"
                                clearable
                            />
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="QQ" prop="qq">
                            <el-input
                                v-model="formData.qq"
                                placeholder="请输入QQ"
                                clearable
                            />
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <!-- <el-form-item label="组织部门">
              <SelectTree
                v-model="formData.orgId"
                :options="orgTreeData"
                :props="defaultProps"
                placeholder="请选择组织部门"
              ></SelectTree>
            </el-form-item> -->
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="性别" prop="sex">
                            <el-radio-group v-model="formData.sex">
                                <el-radio :label="1">女</el-radio>
                                <el-radio :label="0">男</el-radio>
                            </el-radio-group>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="状态" prop="status">
                            <el-radio-group v-model="formData.status">
                                <el-radio :label="1">启用</el-radio>
                                <el-radio :label="0">禁用</el-radio>
                            </el-radio-group>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-form-item label="备注" prop="remark">
                        <el-input
                            v-model="formData.remark"
                            :autosize="{ minRows: 5, maxRows: 8 }"
                            type="textarea"
                            clearable
                            placeholder="请输入备注"
                        />
                    </el-form-item>
                </el-row>
            </el-form>
        </div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="close">取 消</el-button>
            <el-button type="primary" @click="ok">确 定</el-button>
        </div>
    </el-dialog>
</template>
<script>
// import { getOrgTree } from '@/api/sys/org'
// import { addEdit } from '@/api/sys/user'
/**
 * 用户添加编辑
 */
// 手机号验证
const validatePhone = (rule, value, callback) => {
    if (value === '') {
        callback(new Error('请输入手机号'))
    } else {
        if (!/^1[3456789]\d{9}$/.test(value)) {
            callback(new Error('请输入正确的手机号'))
        } else {
            callback()
        }
    }
}
export default {
    name: 'UserAddEdit',
    data() {
        return {
            show: false,
            loading: false,
            disabled: false,
            // 表单数据
            formData: {
                userId: null,
                userName: null,
                password: null,
                email: null,
                phone: null,
                sex: 1,
                status: 1,
                remark: null,
                orgId: null,
                qq: null
            },
            formRules: {
                userName: [
                    { required: true, message: '请输入用户名', trigger: 'blur' }
                ],
                password: [
                    { required: true, message: '请输入密码', trigger: 'blur' }
                ],
                email: [
                    { required: true, message: '请输入邮箱', trigger: 'blur' },
                    {
                        type: 'email',
                        message: '请输入正确的邮箱地址',
                        trigger: ['blur', 'change']
                    }
                ],
                phone: [
                    { required: true, message: '请输入电话', trigger: 'blur' },
                    { validator: validatePhone, trigger: 'change' }
                ]
            }
            //   orgTreeData: [],
            //   defaultProps: {
            //     parent: "parentId", // 父级唯一标识
            //     value: "id", // 唯一标识
            //     label: "label", // 标签显示
            //     children: "children" // 子级
            //   }
        }
    },
    computed: {
        title() {
            return this.formData.userId ? '编辑' : '添加'
        }
    },
    watch: {
        'formData.phone': function(curVal) {
            if (!curVal) {
                this.formData.phone = null
                return false
            }
            // 实时把非数字的输入过滤掉
            this.formData.phone = curVal.match(/\d/gi)
                ? curVal.match(/\d/gi).join('')
                : ''
        }
    },
    mounted() {
        // this.loadOrgTree();
    },
    methods: {
        // 确定
        ok: function() {
            this.$refs.form.validate(valid => {
                if (valid) {
                    this.loading = true
                    this.$api
                        .post(
                            `${process.env.VUE_APP_API_ROOT}api/system/getuserlist`,
                            this.formData
                        )
                        .then(res => {
                            if (res.success) {
                                this.$message.success(res.data)
                                this.$emit('on-ok') // 成功响应事件
                                this.close()
                            } else {
                                this.$message.error(res.msg)
                            }
                        })
                        .catch(() => (this.loading = false))
                }
            })
        },
        // 关闭窗口
        close: function() {
            this.loading = false
            this.show = false
        },
        // 打开窗口
        open: function(row) {
            this.show = true
            this.$nextTick(() => {
                this.$refs.form.resetFields()
                this.formData.userId = null
                this.disabled = false
                if (row) {
                    this.disabled = true
                    this.formData.userId = row.userId
                    this.formData.userName = row.userName
                    this.formData.email = row.email
                    this.formData.phone = row.phone
                    this.formData.sex = row.sex
                    this.formData.status = row.status
                    this.formData.remark = row.remark
                    this.formData.qq = row.qq
                }
            })
        }
    }
}
</script>
<style>
.form-editor {
    padding-right: 5px;
}
</style>
