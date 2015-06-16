// SpaceTest.cs
// Nebula
//
// Created by Oleg Zhelestcov on Wednesday, October 29, 2014 4:13:32 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
namespace Nebula.Test {
    using global::Game.Space;
    using Nebula;
    using System.Collections;
    using UnityEngine;

    public class SpaceTest : Singleton<SpaceTest> {

        public IEnumerator CorTestSkill() {
            NRPC.TestApplyTimedBuffToTarget(120);
            yield return new WaitForSeconds(5);
            NRPC.TestUseSkill("SA030003");
            yield return new WaitForSeconds(5);
            NRPC.TestGetTimedBuffOnTarget();
        }
    }
}
