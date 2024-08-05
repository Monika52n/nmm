using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NMMModel.Model
{
    public class Field
    {
        public ColorFields player { get; set; }
        private Field? up_;
        private Field? down_;
        private Field? left_;
        private Field? right_;
        public Field Up
        {
            get { return up_!; }
            set
            {
                up_ = value;
                value.down_ = this;
            }
        }
        public Field Down
        {
            get { return down_!; }
            set
            {
                down_ = value;
                value.up_ = this;
            }
        }
        public Field Left
        {
            get { return left_!; }
            set
            {
                left_ = value;
                value.right_ = this;
            }
        }
        public Field Right
        {
            get { return right_!; }
            set
            {
                right_ = value;
                value.left_ = this;
            }
        }

        public Field()
        {
            player = ColorFields.TRANSP;
        }
    }
}
