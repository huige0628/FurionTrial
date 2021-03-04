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
            <el-form ref="form" :model="formData" :rules="formRules" label-width="100px">
                <el-form-item label="国际代码" prop="currencyCode">
                    <el-input v-model="formData.currencyCode" placeholder="请输入国际代码" clearable />
                </el-form-item>

                <el-form-item label="货币名称" prop="currencyName">
                    <el-input v-model="formData.currencyName" placeholder="请输入货币名称" clearable />
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
// import { addEdit } from "@/api/sys/role";
/**
 * 用户添加编辑
 */
export default {
    components: {},
    data() {
        return {
            show: false,
            loading: false,
            // 表单数据
            formData: {
                currencyId: null,
                currencyName: null,
                currencyCode: null
            },
            formRules: {
                currencyName: [
                    { required: true, message: '请输入货币名称', trigger: 'blur' }
                ],
                currencyCode: [
                    { required: true, message: '请输入国际代码', trigger: 'blur' },
                    { min: 3, max: 3, message: '长度为3个字符', trigger: 'blur' }
                ]
            }
        }
    },
    computed: {
        title() {
            return this.formData.currencyId ? '编辑' : '添加'
        }
    },
    mounted() {},
    methods: {
        // 确定
        ok: function() {
            this.$refs.form.validate(valid => {
                if (valid) {
                    // this.loading = true;
                    // save
                    this.$message.success('验证成功')
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
                this.formData.currencyId = null
                if (row) {
                    this.formData.currencyId = row.currencyId
                    this.formData.currencyName = row.currencyName
                    this.formData.currencyCode = row.currencyCode
                }
            })
        }
    }
}
</script>
