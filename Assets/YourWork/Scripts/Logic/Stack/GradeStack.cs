using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jenga.Logic.Stack
{
    public class GradeStack : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _gradeText;
        public TMP_Text GradeText => _gradeText;

        public List<StackBrick> Bricks { get; set; } = new();
    }
}
