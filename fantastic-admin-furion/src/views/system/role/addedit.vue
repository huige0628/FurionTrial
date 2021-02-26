<template>
    <el-dialog
        :visible.sync="show"
        :close-on-click-modal="false"
        :title="title"
        width="30%"
        top="10vh"
        scrollable
    >
        <div style="max-height: 550px; overflow-x: auto;" class="form-container">
            <el-form
                ref="form"
                :model="formData"
                :rules="formRules"
                label-width="100px"
            >
                <el-form-item label="角色名称" prop="roleName">
                    <el-input
                        v-model="formData.roleName"
                        placeholder="请输入角色名称"
                        clearable
                    />
                </el-form-item>
                <el-form-item label="状态">
                    <el-radio-group v-model="formData.isEnabled">
                        <el-radio :label="true">启用</el-radio>
                        <el-radio :label="false">禁用</el-radio>
                    </el-radio-group>
                </el-form-item>
                <el-form-item label="备注" prop="remark">
                    <el-input
                        v-model="formData.remark"
                        :autosize="{ minRows: 5, maxRows: 8 }"
                        type="textarea"
                        placeholder="请输入备注"
                    />
                </el-form-item>
            </el-form>
        </div>
        <div slot="footer" class="dialog-footer">
            <el-button @click="close">取 消</el-button>
            <el-button type="primary" @click="ok">确 定</el-button>
        </div>
    </el-dialog>
</template>
<script>
/**
 * 用户添加编辑
 */
export default {
    name: 'RoleAddEdit',
    data() {
        return {
            show: false,
            loading: false,
            // 表单数据
            formData: {
                roleId: null,
                roleName: null,
                isEnabled: true,
                remark: null
            },
            formRules: {
                roleName: [
                    { required: true, message: '请输入角色名称', trigger: 'blur' }
                ]
            }
        }
    },
    computed: {
        title() {
            return this.formData.userId ? '编辑' : '添加'
        }
    },
    mounted() {},
    methods: {
        // 确定
        ok: function() {
            this.$refs.form.validate(valid => {
                if (valid) {
                    this.loading = true
                    this.$api
                        .post('api/system/roleaddedit', this.formData)
                        .then(res => {
                            if (res.succeeded) {
                                this.$message.success('保存成功')
                                this.$emit('on-ok') // 成功响应事件
                                this.close()
                            } else {
                                this.$message.error(res.errors)
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
                this.formData.roleId = null
                if (row) {
                    this.formData.roleId = row.roleId
                    this.formData.roleName = row.roleName
                    this.formData.isEnabled = row.isEnabled
                    this.formData.remark = row.remark
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
