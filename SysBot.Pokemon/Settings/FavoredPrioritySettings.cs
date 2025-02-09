﻿using System;
using System.ComponentModel;

namespace SysBot.Pokemon
{
    public class FavoredPrioritySettings : IFavoredCPQSetting
    {
        private const string Operation = nameof(Operation);
        private const string Configure = nameof(Configure);
        public override string ToString() => "偏好设置Favoritism Settings";

        // We want to allow hosts to give preferential treatment, while still providing service to users without favor.
        // These are the minimum values that we permit. These values yield a fair placement for the favored.
        private const int _mfi = 15;
        private const float _bmin = 1;
        private const float _bmax = 3;
        private const float _mexp = 0.5f;
        private const float _mmul = 0.1f;

        private int _minimumFreeAhead = _mfi;
        private float _bypassFactor = 1.5f;
        private float _exponent = 0.777f;
        private float _multiply = 0.5f;

        [Category(Operation), Description("确定如何计算喜欢的用户的插入位置。\"None\" 将防止应用任何偏袒\nDetermines how the insertion position of favored users is calculated. \"None\" will prevent any favoritism from being applied.")]
        public FavoredMode Mode { get; set; }

        [Category(Configure), Description("插入到(不受欢迎的用户)^(指数)不受欢迎的用户之后\nInserted after (unfavored users)^(exponent) unfavored users.")]
        public float Exponent
        {
            get => _exponent;
            set => _exponent = Math.Max(_mexp, value);
        }

        [Category(Configure), Description("相乘:插入在(不喜欢的用户)*(相乘)不喜欢的用户之后\nMultiply: Inserted after (unfavored users)*(Multiply) unfavored users.")]
        public float Multiply
        {
            get => _multiply;
            set => _multiply = Math.Max(_mmul, value);
        }

        [Category(Configure), Description("不受欢迎的用户数量不能跳过。只有当队列中有大量不受欢迎的用户时，才会强制执行此操作\nAmount of unfavored users to not skip over. This only is enforced if a significant amount of unfavored users are in the queue.")]
        public int MinimumFreeAhead
        {
            get => _minimumFreeAhead;
            set => _minimumFreeAhead = Math.Max(_mfi, value);
        }

        [Category(Configure), Description("队列中不受欢迎的用户的最小数量导致{MinimumFreeAhead}被强制执行。当上述金额高于此值时，受欢迎的用户不会排在{MinimumFreeAhead}不受欢迎的用户前面\nMinimum amount of unfavored users in queue to cause {MinimumFreeAhead} to be enforced. When the aforementioned amount is higher than this value, a favored user is not placed ahead of {MinimumFreeAhead} unfavored users.")]
        public int MinimumFreeBypass => (int)Math.Ceiling(MinimumFreeAhead * MinimumFreeBypassFactor);

        [Category(Configure), Description("与{MinimumFreeAhead}相乘以确定{MinimumFreeBypass}的值\nScalar that is multiplied with {MinimumFreeAhead} to determine the {MinimumFreeBypass} value.")]
        public float MinimumFreeBypassFactor
        {
            get => _bypassFactor;
            set => _bypassFactor = Math.Min(_bmax, Math.Max(_bmin, value));
        }
    }
}
